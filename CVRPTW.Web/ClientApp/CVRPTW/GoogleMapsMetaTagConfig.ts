import { LazyMapsAPILoaderConfigLiteral } from '@agm/core';
import { Injectable } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Injectable()
export class GoogleMapsMetaTagConfig implements LazyMapsAPILoaderConfigLiteral {
  public apiKey: string | undefined;
  public libraries: string[];

  constructor(meta: Meta) {
    this.apiKey = meta.getTag("name=google-maps-api-key")?.content;
    this.libraries = ["places", "geometry"];
  }
}
