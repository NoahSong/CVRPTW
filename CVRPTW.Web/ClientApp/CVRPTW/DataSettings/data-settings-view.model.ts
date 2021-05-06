export class DataSettingsViewModel {
  //public selectedTestOption!: DataSettingsViewModel_TestOptionModel;
  public selectedTestOption!: string;
  public testOptions!: DataSettingsViewModel_TestOptionModel[];
  public error!: string;
}

export class DataSettingsViewModel_TestOptionModel {
  public title!: string;
  public value!: string;
  public isCustomisable!: boolean;
}
