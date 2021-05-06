import { NumberValueAccessor } from "@angular/forms";

export class VehicleRoutingModel {
  public bookings: VehicleRoutingModel_Booking[] = [];
  public totalDuration!: number;
  public depot!: VehicleRoutingModel_Depot;
}

export class VehicleRoutingModel_Booking {
  public location!: VehicleRoutingModel_Location;
  public serviceFromTime!: Date;
  public serviceToTime!: Date;
  public serviceMins!: number;
  public fuelTypes!: string[];
  public title: string = "";
  public points!: VehicleRoutingModel_Point[];
  public order!: number;
  public nextNodeIndex!: number;
}

export class VehicleRoutingModel_Point {
  public duration!: number;
  public latitude!: number;
  public longitude!: number;
}

export class VehicleRoutingModel_Location {
  public latitude!: number;
  public longitude!: number;
}

export class VehicleRoutingModel_Depot {
  public location: VehicleRoutingModel_Location | undefined;
  public vehicles: VehicleRoutingModel_Depot_Vehicle[] = [];
}

export class VehicleRoutingModel_Depot_Vehicle {
  public name: string = "";
  public currentLocation: VehicleRoutingModel_Location | undefined;
  public ordinalBookings: VehicleRoutingModel_Booking[] = [];
  public totalDuration: number = 0;
  public containers: VehicleRoutingModel_Deport_Vehicle_Container[] = [];
}

export class VehicleRoutingModel_Deport_Vehicle_Container {
  public fuelTypes: string[] = [];
  public capacity: number | undefined;
}
