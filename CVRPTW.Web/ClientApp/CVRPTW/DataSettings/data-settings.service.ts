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

  public getTestDataSet(option: string, optionRoutingApi: string, optionSolver: string): Observable<VehicleRoutingModel> {
    return this.httpClient
      //.get("api/datasets/" + option + "/" + optionRoutingApi + `0${month}`)
      .get(`api/datasets/${option}/${optionRoutingApi}/${optionSolver}`)
      .pipe(map(response =>
      {
        let stop:number = 1; 
        return (response as VehicleRoutingModel)
      }
        
      ));
  }
}
