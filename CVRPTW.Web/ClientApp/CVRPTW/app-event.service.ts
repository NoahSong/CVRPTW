import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { VehicleRoutingModel } from './DataSettings/vehicle-routing.model';

@Injectable()
export class AppEventService {
  public vehicleRoutingDataChanged: Subject<VehicleRoutingModel>;

  constructor() {
    this.vehicleRoutingDataChanged = new Subject<VehicleRoutingModel>();
  }
}
