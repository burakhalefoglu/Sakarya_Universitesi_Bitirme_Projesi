import {Component, OnDestroy, OnInit} from '@angular/core';
import * as Chartist from 'chartist';
import {DashboardService} from './service/dashboard.service';
import {ClientModel} from '../../models/client-model';
import {ApiInfoModel} from '../../models/api-info-model';
import {HelperService} from './service/helper.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
    public totalDataCount: number;
    public apiInfoModel: ApiInfoModel;
    public totalPleasedRate: number;
    public clientModels: Array<ClientModel>;
    public clientRateByDates: Array<number>;

   interval: any;
  constructor(private dashboardService: DashboardService,
              private helperService: HelperService) { }

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
  }
  ngOnInit() {
      this.apiInfoModel = new ApiInfoModel();
      this.clientModels = new Array<ClientModel>();
      this.clientRateByDates = new Array<number>();

      this.getLastClientModels();
      this.getPositiveSentimentRate();
      this.getPositiveSentimentRateByDate();
      this.getTotalClientCount();
      this.getApiInfo();
      this.getAllRequestIntervalAsync().then(r => {})
  }

    async getAllRequestIntervalAsync() {
        this.interval = setInterval(() => {
            this.getLastClientModels();
            this.getPositiveSentimentRate();
            this.getPositiveSentimentRateByDate();
            this.getTotalClientCount();
            this.getApiInfo();
        }, 10000);
    }


    getTotalClientCount() {
        this.dashboardService.getTotalClientCount().subscribe(data => {
            if (data.success) {
                this.totalDataCount = data.data;
            }
        })

    }

    getLastClientModels() {
        this.dashboardService.getLastClientsByCount(10).subscribe(data => {
            if (data.success) {
                this.clientModels = data.data;
            }
        })
    }

    getPositiveSentimentRate() {
        this.dashboardService.getPositiveSentimentRate().subscribe(data => {
            if (data.success) {
               this.totalPleasedRate = data.data;
            }
        })
    }

    getPositiveSentimentRateByDate() {
      const filters = this.helperService.getDateFilters();
      console.log(Date.now().toString())
      console.log(filters)
        for (const filter of filters) {
            this.clientRateByDates = new Array<number>()
        this.dashboardService.getPositiveSentimentRateByDate(filter[0], filter[1]).subscribe(data => {
            if (data.success) {
                this.clientRateByDates.push(data.data);
            }
        })
        }
        console.log(this.clientRateByDates)
        this.dateBaseChartInit(this.clientRateByDates);
    }

    getApiInfo() {
        this.dashboardService.getApiInfo('TwitterApi').subscribe(data => {
            if (data.success) {
                this.apiInfoModel = new ApiInfoModel();
                this.apiInfoModel = data.data
                this.apiInfoModel.truePositive = this.apiInfoModel.truePositive / this.apiInfoModel.totalData * 100
                this.apiInfoModel.trueNegative = this.apiInfoModel.trueNegative / this.apiInfoModel.totalData * 100
                this.apiInfoModel.falsePositive = this.apiInfoModel.falsePositive / this.apiInfoModel.totalData * 100
                this.apiInfoModel.falseNegative = this.apiInfoModel.falseNegative / this.apiInfoModel.totalData * 100
                this.statisticChartInit([this.apiInfoModel.truePositive,
                this.apiInfoModel.trueNegative,
                this.apiInfoModel.falsePositive,
                this.apiInfoModel.falseNegative]);


            }
        })
    }

    private statisticChartInit(arr: Array<number>) {
        const dataDateBasePleasedGraph = {
            labels: ['True Positive', 'True Negative', 'False Positive', 'False Negative'],
            series: [arr]
        };
        const optionsDateBasePleasedGraph = {
            axisX: {
                showGrid: false
            },
            low: 0,
            high: 10,
            chartPadding: {top: 0, right: 5, bottom: 0, left: 20}
        };
        const responsiveOptions: any[] = [
            ['screen and (max-width: 640px)', {
                seriesBarDistance: 20,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value[0];
                    }
                }
            }]
        ];
        const chart = new Chartist.Bar('#statisticAdvGraph', dataDateBasePleasedGraph,
            optionsDateBasePleasedGraph, responsiveOptions);
        this.startAnimationForBarChart(chart);
    }

    private dateBaseChartInit(series: Array<number>) {
        const dataDateBasePleasedGraph = {
            labels: ['Last Week', 'Last Mount', 'Last 3 Mount', 'Last 6 Mount'],
            series: [
                series
            ]
        };

        const optionsDateBasePleasedGraph = {
            axisX: {
                showGrid: false
            },
            low: 0,
            high: 100,
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
        const chart = new Chartist.Bar('#dateBasePleasedGraph', dataDateBasePleasedGraph,
            optionsDateBasePleasedGraph, responsiveOptions);
        this.startAnimationForBarChart(chart);
    }
    ngOnDestroy(): void {
        clearInterval( this.interval);
    }
}
