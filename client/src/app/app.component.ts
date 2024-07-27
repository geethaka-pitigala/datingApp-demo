import { HttpClient, provideHttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: []
})
export class AppComponent implements OnInit{
  title = 'client';
  users: any;
  http = inject(HttpClient);

  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/users").subscribe({
      next: response => {
        this.users = response;
      },
      error: error => {
        console.warn(error);
      }
    });
  }
}
