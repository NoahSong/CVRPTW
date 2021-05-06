import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { VehicleRoutingModel } from './vehicle-routing.model';

@Injectable()
export class DataSettingsService {
  private httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  public getTestDataSet(option: string): Observable<VehicleRoutingModel> {
    return this.httpClient
      .get("api/datasets/" + option)
      .pipe(map(response => response as VehicleRoutingModel));
  }
}
