import { TestBed, getTestBed  } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { FlightsService } from './flights.service';

describe('FlightsService', () => {
  let injector: TestBed;
  let httpMock: HttpTestingController;
  let service: FlightsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FlightsService]
    });
    injector = getTestBed();
    service = injector.get(FlightsService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return an Observable<Flight[]>', () => {
    const dummyFlights = [
      { flightId: 'ID1' },
      { flightId: 'ID2' }
    ];

    service.get().subscribe(flights => {
      expect(flights.length).toBe(2);
      expect(flights).toEqual(flights);
    });

    const req = httpMock.expectOne(`${service.accessPointUrl}`);
    expect(req.request.method).toBe("GET");
    req.flush(dummyFlights);
  });

});
