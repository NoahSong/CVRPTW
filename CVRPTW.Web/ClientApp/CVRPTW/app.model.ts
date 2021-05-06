import { VehicleRoutingModel } from "./DataSettings/vehicle-routing.model";

export class AppModel {
  public mapLatitude: number | undefined;
  public mapLongitude: number | undefined;
  public mapZoomLevel: number | undefined;
  public mapStyle: google.maps.MapTypeStyle[] = [{
    featureType: "poi",
    elementType: "all",
    stylers: [{
      visibility: "off"
    }]
  }];
  public vehicleRoutingModel: VehicleRoutingModel | undefined;
}
