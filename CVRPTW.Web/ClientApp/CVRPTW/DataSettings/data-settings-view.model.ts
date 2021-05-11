import { RoutingOptions } from "CVRPTW/Enums/routing-options";
import { SolverOptions } from "CVRPTW/Enums/solver-options";

export class DataSettingsViewModel {
  //public selectedTestOption!: DataSettingsViewModel_TestOptionModel;
  public selectedTestOption!: string;
  public testOptions!: DataSettingsViewModel_TestOptionModel[];
  public selectedTestApiRoutingOption!: string;
  public testApiRoutingOption!: DataSettingsViewModel_TestApiRoutingOptionModel[];
  public selectedTestSolverOption!: string;
  public testSolverOption!: DataSettingsViewModel_TestSolverOptionModel[];
  public error!: string;
}

export class DataSettingsViewModel_TestOptionModel {
  public title!: string;
  public value!: string;
  public isCustomisable!: boolean;
}
export class DataSettingsViewModel_TestApiRoutingOptionModel {
  public title!: string;
  public value!: RoutingOptions;
  public isCustomisable!: boolean;
}
export class DataSettingsViewModel_TestSolverOptionModel {
  public title!: string;
  public value!: SolverOptions;
  public isCustomisable!: boolean;
}
