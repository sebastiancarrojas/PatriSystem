export interface DailySales {
  date: string;
  amount: number;
}

export interface TopProduct {
  productName: string;
  quantity: number;
  revenue: number;
}

export interface Dashboard {
  salesTodayCount: number;
  salesTodayAmount: number;
  salesWeekCount: number;
  salesWeekAmount: number;
  salesMonthCount: number;
  salesMonthAmount: number;
  totalProducts: number;
  last7DaysSales: DailySales[];
  topProducts: TopProduct[];
}