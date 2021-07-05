import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent {
  public airingMovies: Movie[];

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http.get<Movie[]>(baseUrl + "Movie/movies").subscribe(
      (result) => {
        this.airingMovies = result;
      },
      (error) => console.error(error)
    );
  }
}

export interface Movie {
  movieId: number;
  movieName: string;
  releaseDate: string;
  language: string;
  genre: string;
  price: string;
}
