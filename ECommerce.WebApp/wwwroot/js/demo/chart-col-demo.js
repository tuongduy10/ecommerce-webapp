var chart = new CanvasJS.Chart("chartContainer", {
	animationEnabled: true,
	title:{
		text: "Thông kê năm 2021"
	},	
	axisY: {
		title: "Doanh thu",
		titleFontColor: "#4F81BC",
		lineColor: "#4F81BC",
		labelFontColor: "#4F81BC",
		tickColor: "#4F81BC"
	},
	toolTip: {
		shared: true
	},
	legend: {
		cursor:"pointer",
		itemclick: toggleDataSeries
	},
	data: [{
		type: "column",
		name: "Doanh thu",
		legendText: "Doanh thu",
		showInLegend: true, 
		dataPoints:[
			{ label: "Tháng 01", y: 10000000  },
			{ label: "Tháng 02", y: 10000000 },
			{ label: "Tháng 03", y: 10000000 },
			{ label: "Tháng 04", y: 10000000 },
			{ label: "Tháng 05", y: 10000000 },
			{ label: "Tháng 06", y: 10000000 },
      		{ label: "Tháng 07", y: 10000000 },
			{ label: "Tháng 08", y: 10000000 },
			{ label: "Tháng 09", y: 10000000 },
		]
	},
	{
		type: "column",	
		name: "Đơn hàng",
		legendText: "Đơn hàng",
		axisYType: "secondary",
		showInLegend: true,
		dataPoints:[
			{ label: "Tháng 01", y: 100 },
			{ label: "Tháng 02", y: 200 },
			{ label: "Tháng 03", y: 400 },
			{ label: "Tháng 04", y: 500 },
			{ label: "Tháng 05", y: 200 },
			{ label: "Tháng 06", y: 200},
      		{ label: "Tháng 07", y: 800 },
			{ label: "Tháng 08", y: 200 },
			{ label: "Tháng 09", y: 300 },
		]
	},
  	{
		type: "column",	
		name: "Người dùng",
		legendText: "Người dùng",
		axisYType: "tertiary",
		showInLegend: true,
		dataPoints:[
			{ label: "Tháng 01", y: 10 },
			{ label: "Tháng 02", y: 21 },
			{ label: "Tháng 03", y: 31 },
			{ label: "Tháng 04", y: 41 },
			{ label: "Tháng 05", y: 21 },
			{ label: "Tháng 06", y: 12 },
      		{ label: "Tháng 07", y: 10 },
			{ label: "Tháng 08", y: 21 },
			{ label: "Tháng 09", y: 3 },			
		]
	}]
});
chart.render();

function toggleDataSeries(e) {
	if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
		e.dataSeries.visible = false;
	}
	else {
		e.dataSeries.visible = true;
	}
	chart.render();
}