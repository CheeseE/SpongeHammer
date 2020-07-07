import { Component, OnInit, Input  } from '@angular/core';

import { FlightsService, Flight } from '../flights.service'

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  public flightsData: Array<Flight>;
  public flightInfo: Flight;
  public searchString: string;

  constructor(private flightService: FlightsService) { 
    flightService.get().subscribe((data: Flight[]) => this.flightsData = data);
  }

  ngOnInit(): void {
  }

}
