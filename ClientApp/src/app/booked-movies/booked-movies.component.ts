import { HttpClient } from "@angular/common/http";
import { Movie } from "./../home/home.component";
import { Component, Inject } from "@angular/core";

@Component({
  selector: "app-booked-movies",
  templateUrl: "./booked-movies.component.html",
})
export class BookedMoviesComponent {
  public BookedMovies: Movie[];

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http.get<Movie[]>(baseUrl + "userbag/usermovies").subscribe(
      (result) => {
        this.BookedMovies = result;
      },
      (error) => console.error(error)
    );
  }
}
