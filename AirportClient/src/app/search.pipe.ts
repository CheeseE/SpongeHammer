import { Pipe, PipeTransform, Injectable } from '@angular/core';

@Pipe({
  name: 'filter'
})
@Injectable()
export class SearchPipe implements PipeTransform {
  transform(items: any[],  value: string): any[] {
    if (!items) {
      return [];
    }
    if (!value) {
      return items;
    }

    return items.filter(singleItem =>
      singleItem["airport"].toLowerCase().includes(value.toLowerCase()) ||
      singleItem["flightId"].toLowerCase().includes(value.toLowerCase()) ||
      singleItem["terminal"].toLowerCase().includes(value.toLowerCase()) ||
      singleItem["arrivingFrom"].toLowerCase().includes(value.toLowerCase()) ||
      singleItem["status"].toLowerCase().includes(value.toLowerCase())
    );
  }
}