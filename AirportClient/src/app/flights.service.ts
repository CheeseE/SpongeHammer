import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface Flight {
  airport: string;
  terminal: string;
  flightId: string;
  scheduled: string;
  arrivingFrom: string;
  status:string;
}

@Injectable()
export class FlightsService {

  private headers: HttpHeaders;
  public accessPointUrl: string = 'https://localhost:5001/api/Flights';

  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8'});
  }

  public get() {
    // Get all jogging data
    return this.http.get<Flight[]>(this.accessPointUrl, {headers: this.headers});
  }
}
