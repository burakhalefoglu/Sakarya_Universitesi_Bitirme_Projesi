import {Component, OnDestroy, OnInit} from '@angular/core';
import * as Chartist from 'chartist';
import {DashboardService} from './service/dashboard.service';
import {ClientModel} from '../../model/client-model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
   tweetsCount: number;
   mlPredictionRate: number;
   totalRequest: number;
   usedRequest: number;
   totalPleasedRate: number;
   clientModels: Array<ClientModel>;
    interval1: any;
    interval2: any;


  constructor(private dashboardService: DashboardService) { }

  startAnimationForBarChart(chart) {
      let seq2: any, delays2: any, durations2: any;

      seq2 = 0;
      delays2 = 80;
      durations2 = 500;
      chart.on('draw', function(data) {
        if (data.type === 'bar') {
            seq2++;
            data.element.animate({
              opacity: {
                begin: seq2 * delays2,
                dur: durations2,
                from: 0,
                to: 1,
                easing: 'ease'
              }
            });
        }
      });

      seq2 = 0;
  };

  ngOnInit() {
      this.clientModels = new Array<ClientModel>();
      this.getApiInfo().then(r => {});
      this.getClientModels().then(r => {});
  }

    async getClientModels() {
        this.interval1 = setInterval(() => {
            this.dashboardService.getAllClients().subscribe(data => {
                if (data.success) {
                    this.tweetsCount = data.data.length;
                    this.calculateTotalPleasedRate();
                    this.barChartInit();
                }
            })
        }, 10000);
    }

    async getApiInfo() {
        this.interval2 = setInterval(() => {
            this.dashboardService.getApiInfo('TwitterApi').subscribe(data => {
                if (data.success) {
                    this.mlPredictionRate = data.data.mlResultRate
                    this.totalRequest = data.data.totalRequestLimit;
                    this.usedRequest = data.data.remainingRequest;
                }
            })
        }, 10000);
    }

    private barChartInit() {
        const label: Array<string> = new Array<string>();
        const series: Array<number> = new Array<number>();
        const lastTenDate: Array<Date> = new Array<Date>();

        this.setLabel(label);
        this.setLastTenDays(lastTenDate);
        this.setSeries(lastTenDate, series);

        const dataDateBasePleasedGraph = {
            labels: label,
            series: [
                series
            ]
        };

        const optionsDateBasePleasedGraph = {
            axisX: {
                showGrid: false
            },
            low: 0,
            high: 1000,
            chartPadding: {top: 0, right: 5, bottom: 0, left: 0}
        };
        const responsiveOptions: any[] = [
            ['screen and (max-width: 640px)', {
                seriesBarDistance: 5,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value[0];
                    }
                }
            }]
        ];
        const websiteViewsChart = new Chartist.Bar('#dateBasePleasedGraph', dataDateBasePleasedGraph,
            optionsDateBasePleasedGraph, responsiveOptions);
        this.startAnimationForBarChart(websiteViewsChart);
    }

    private setSeries(lastTenDate: Array<Date>, series: Array<number>) {
        for (let i = 0; i < lastTenDate.length; i++) {
            const date = lastTenDate[i];
            this.clientModels.forEach(x => {
                if (x.dateTime === date) {
                    series[i] += 1;
                }
            })
        }
    }

    private setLastTenDays(lastTenDate: Array<Date>) {
        for (let i = 0; i < 10; i++) {
            const d = new Date();
            d.setDate(d.getDate() - i);
            lastTenDate.push(d);
        }
    }

    private setLabel(label: Array<string>) {
        for (let i = 0; i < 10; i++) {
            const day = new Date();
            day.setDate(day.getDate() - i);
            const date = day.getFullYear() + '-' + day.getMonth() + '-' + day.getDay()
            label.push(date)
        }
    }

    private calculateTotalPleasedRate() {
        const totalClientCount = this.clientModels.length;
        let pleasedCount = 0;
        for (const element of this.clientModels) {
            if (element.isPleased === 1) {
                pleasedCount ++;
            }
        }
        this.totalPleasedRate = 100 * pleasedCount / totalClientCount;
    }

    ngOnDestroy(): void {
        clearInterval( this.interval1);
        clearInterval( this.interval2);
    }
}
