import { Component, OnInit, OnDestroy } from '@angular/core';
import { TodoListService } from 'src/app/services/todolist.service';
import { TodoItemModel } from 'src/app/models/todoitem.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TodoEditComponent } from '../todo-edit/todo-edit.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-todo-overview',
  templateUrl: './todo-overview.component.html',
  styleUrls: ['./todo-overview.component.less']
})
export class TodoOverviewComponent implements OnInit, OnDestroy {
  private todoItemsChanged: Subscription;
  public todoItems: TodoItemModel[] = [];
  public searchText: string = 'bla';

  constructor(private todoListService: TodoListService, private modalService: NgbModal) { }

  ngOnInit() {
    this.todoItems = this.todoListService.todoItems;

    this.todoItemsChanged = this.todoListService.todoItemsChanged.subscribe((todoItems) => {
      this.todoItems = todoItems;
    });
  }

  ngOnDestroy(): void {
    this.todoItemsChanged.unsubscribe();
  }

  public onEdit(todoItemId: number): void {
    this.openEditPopup(todoItemId);
  }

  public onAdd(): void {
    this.openEditPopup(null);
  }

  public onDelete(todoItemId: number): void {
    this.todoListService.delete(todoItemId);
  }

  public onSearch(): void {
    this.todoListService.search(this.searchText);
  }

  private openEditPopup(todoItemId: number) {
    const modalRef = this.modalService.open(TodoEditComponent);
    const component = <TodoEditComponent>modalRef.componentInstance;
    component.idToEdit.next(todoItemId);
  }
}
