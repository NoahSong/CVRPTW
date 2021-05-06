import { AgmCoreModule, LAZY_MAPS_API_CONFIG } from '@agm/core';
import { AgmOverlays } from 'agm-overlays';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { GoogleMapsMetaTagConfig } from './GoogleMapsMetaTagConfig';
import { DataSettingsComponent } from './DataSettings//data-settings.component';
import { DataSettingsService } from './DataSettings/data-settings.service';
import { HttpClientModule } from '@angular/common/http';
import { AppEventService } from './app-event.service';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    DataSettingsComponent
  ],
  imports: [
    FormsModule,
    AgmOverlays,
    AgmCoreModule.forRoot(),
    BrowserModule,
    RouterModule.forRoot([
      { path: "", pathMatch: "full", redirectTo: "app" },
    ]),
    HttpClientModule
  ],
  providers: [
    AppEventService,
    DataSettingsService,
    { provide: LAZY_MAPS_API_CONFIG, useClass: GoogleMapsMetaTagConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
