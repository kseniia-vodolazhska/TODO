import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.less']
})
export class ErrorComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public onReloadPage(): void {
    location.reload();
  }
}
