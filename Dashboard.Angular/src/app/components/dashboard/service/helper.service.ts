import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  public getDateFilters(): Array<[number, number]> {

    return [ this.getLastWeekFilter(),
      this.getLastMonthByMonthCountFilter(1),
      this.getLastMonthByMonthCountFilter(3),
      this.getLastMonthByMonthCountFilter(6)]
  }

  private getLastWeekFilter(): [number, number] {
    const today = new Date();
    const lastWeek = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
    return [this.convertDateToNumber(today), this.convertDateToNumber(lastWeek)];
  }

  private getLastMonthByMonthCountFilter(month: number): [number, number] {
    const lastMonth = new Date().setMonth(new Date().getMonth() - month);
    return [this.convertDateToNumber(new Date()), this.convertDateToNumber(new Date(1000 * lastMonth))];
  }

private convertDateToNumber(d: Date): number {

  const year = d.getFullYear().toString();
  const month = d.getMonth() < 10 ? '0' + d.getMonth().toString() : d.getMonth().toString();
  const day = d.getDate() < 10 ? '0' + d.getDate().toString() : d.getDate().toString();

  const date = year + month + day
  return  Number(date)
  }
}
