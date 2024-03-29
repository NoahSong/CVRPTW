import { Component, OnInit } from '@angular/core';
import { VehicleRoutingModel, VehicleRoutingModel_Depot_Vehicle } from './vehicle-routing.model';
import { DataSettingsService } from './data-settings.service';
import { AppEventService } from 'CVRPTW/app-event.service';
import { DataSettingsViewModel, DataSettingsViewModel_TestOptionModel } from './data-settings-view.model';
import { RoutingOptions } from 'CVRPTW/Enums/routing-options';
import { SolverOptions } from 'CVRPTW/Enums/solver-options';

@Component({
  selector: 'data-settings',
  templateUrl: './data-settings.component.html'
})
export class DataSettingsComponent implements OnInit {
  private appEventService: AppEventService;
  private dataSettingsService: DataSettingsService;
  public vehicleRoutingModel: VehicleRoutingModel | undefined;

  public viewModel!: DataSettingsViewModel;
  public loading: boolean = false;
  public Math = Math;


  constructor(appEventService: AppEventService,
    dataSettingsService: DataSettingsService) {
    this.appEventService = appEventService;
    this.dataSettingsService = dataSettingsService;
  }

  ngOnInit(): void {
    this.viewModel = new DataSettingsViewModel();
    this.viewModel.testOptions = [
      { title: "Test with 21 static delivery points (VRP)", value: "test-vrp", isCustomisable: false },
      { title: "Test with 30 static delivery points (VRPTW)", value: "test-vrptw", isCustomisable: false },
      { title: "Test with 5 test dataset", value: "test-vrptw/5", isCustomisable: false },
      { title: "Test with 10 test dataset", value: "test-vrptw/10", isCustomisable: false },
      { title: "Test with 100 test dataset", value: "test-vrptw/100", isCustomisable: false },
      { title: "Test with 1000 test dataset", value: "test-vrptw/1000", isCustomisable: false }
    ];
    this.viewModel.testApiRoutingOption = [
      { title: "Here API", value: RoutingOptions.HERE, isCustomisable: false },
      { title: "OSRM API", value: RoutingOptions.OSRM, isCustomisable: false },
      { title: "OpenRouteService", value: RoutingOptions.OPEN_ROUTE_SERVICE, isCustomisable: false },
      { title: "tomtom", value: RoutingOptions.TOMTOM, isCustomisable: false },
      { title: "PGRouting", value: RoutingOptions.PGROUTING, isCustomisable: false },
      { title: "ESRI", value: RoutingOptions.ESRI, isCustomisable: false },
    ];
    this.viewModel.testSolverOption = [
      { title: "OR Tools", value: SolverOptions.ORTOOLS, isCustomisable: false },
      { title: "VROOM", value: SolverOptions.VROOM, isCustomisable: false },
      { title: "PGRouting", value: SolverOptions.PGROUTING, isCustomisable: false },
      { title: "Here API", value: SolverOptions.HERE, isCustomisable: false }
    ];
  }

  public calculateClicked(calculationOption: string, selectedTestApiRoutingOption: string, selectedTestSolverOption: string): void {

    if (!selectedTestApiRoutingOption) {
      this.viewModel.error = "Please select a routing API for the test";
      return;
    }
    if (!selectedTestSolverOption) {
      this.viewModel.error = "Please select a solver";
      return;
    }
    if (!calculationOption) {
      this.viewModel.error = "Please select test type";
    }
    this.loading = true;
    this.dataSettingsService
      .getTestDataSet(calculationOption, selectedTestSolverOption, selectedTestApiRoutingOption)
      .subscribe(vehicleRoutingModel => {
        this.vehicleRoutingModel = vehicleRoutingModel;
        this.appEventService.vehicleRoutingDataChanged.next(this.vehicleRoutingModel);
      },
        () => { },
        () => { this.loading = false; });
  }

  public getBookingOrders(vehicle: VehicleRoutingModel_Depot_Vehicle): string {
    if (vehicle != undefined && vehicle.ordinalBookings != undefined && vehicle.ordinalBookings.length > 0) {
      let orderStr = "";

      for (let i = 0; i < vehicle.ordinalBookings.length; i++) {
        const booking = vehicle.ordinalBookings[i];
        if (i == vehicle.ordinalBookings.length - 1) {
          orderStr += booking.title;
        } else {
          orderStr += booking.title + " > ";
        }
      }

      return orderStr;
    }

    return "No order found";
  }
}
