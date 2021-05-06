import { Component, OnDestroy, OnInit } from '@angular/core';
import { Meta } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { AppEventService } from './app-event.service';
import { AppModel } from './app.model';
import { VehicleRoutingModel } from './DataSettings/vehicle-routing.model';

@Component({
  selector: 'app',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  private appEventService: AppEventService;
  private vehicleRoutingModelChangedSubscription!: Subscription;

  public apiKey: string | undefined;
  public viewModel: AppModel;
  public vehicleRoutingModel: VehicleRoutingModel | undefined;

  constructor(
    appEventService: AppEventService,
    meta: Meta) {
    this.appEventService = appEventService;

    this.apiKey = meta.getTag("name=google-maps-api-key")?.content;
    this.viewModel = new AppModel();
  }

  ngOnInit(): void {
    this.vehicleRoutingModelChangedSubscription = this.appEventService
      .vehicleRoutingDataChanged
      .subscribe(vehicleRoutingModel => {
        this.vehicleRoutingModel = vehicleRoutingModel;
      });

    if (navigator) {
      navigator.geolocation.getCurrentPosition(pos => {
        this.viewModel.mapLatitude = pos.coords.latitude;
        this.viewModel.mapLongitude = pos.coords.longitude;
        this.viewModel.mapZoomLevel = 16;
      });
    }
  }

  ngOnDestroy(): void {
    this.vehicleRoutingModelChangedSubscription?.unsubscribe()
  }
}
