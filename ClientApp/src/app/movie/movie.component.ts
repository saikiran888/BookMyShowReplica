import { ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";
import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Movie } from "../home/home.component";

@Component({
  selector: "app-movie",
  templateUrl: "./movie.component.html",
  styleUrls: ["./movie.component.css"],
})
export class MovieComponent implements OnInit {
  public movie: Movie;
  public _id: number;
  public _baseUrl: string;
  public _http: HttpClient;

  constructor(
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string,
    private route: ActivatedRoute
  ) {
    this._http = http;
    this._baseUrl = baseUrl;
    this._id = +this.route.snapshot.paramMap.get("id");
    http.get<Movie>(baseUrl + "Movie/movies/id=" + this._id).subscribe(
      (result) => {
        this.movie = result;
      },
      (error) => console.error(error)
    );
  }
  ngOnInit(): void {}
  public AddMovie(): any {
    this._http
      .post(this._baseUrl + "UserBag/usermovies/" + this._id, this._id)
      .subscribe();
    var htmlelement = document.getElementById("info");
    htmlelement.style.display = "block";
  }
  public Close(): any {
    document.getElementById("info").style.display = "none";
  }
}
