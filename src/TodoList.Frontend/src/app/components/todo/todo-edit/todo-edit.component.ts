import { Component, OnInit, OnDestroy } from '@angular/core';
import { TodoListService } from 'src/app/services/todolist.service';
import { BehaviorSubject } from 'rxjs';
import { TodoItemModel } from 'src/app/models/todoitem.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-todo-edit',
  templateUrl: './todo-edit.component.html',
  styleUrls: ['./todo-edit.component.less']
})
export class TodoEditComponent implements OnInit, OnDestroy {
  public editForm: FormGroup;
  public idToEdit = new BehaviorSubject<number>(null);
  public editItem: TodoItemModel;

  constructor(public activeModal: NgbActiveModal, private todoListService: TodoListService) { }

  ngOnInit() {
    this.initEditForm();

    this.idToEdit.subscribe((id: number) => {
      if (id) {
        this.editItem = this.todoListService.get(id);
        this.editForm.setValue({
          'title': this.editItem.title,
          'description': this.editItem.description
        });
      } else {
        this.editItem = new TodoItemModel();
        this.editForm.reset();
      }
    });
  }

  ngOnDestroy(): void {
    this.idToEdit.unsubscribe();
  }

  private initEditForm(): void {
    this.editForm = new FormGroup({
      'title': new FormControl(null, [Validators.required]),
      'description': new FormControl(null, [Validators.required])
    });
  }

  public onSave(): void {
    if (this.editForm.invalid) {
      return;
    }

    this.editItem.title = this.editForm.value.title;
    this.editItem.description = this.editForm.value.description;

    this.todoListService.save(this.editItem).subscribe(() => {
      this.editForm.reset();
      this.activeModal.close();
    });
  }
}
