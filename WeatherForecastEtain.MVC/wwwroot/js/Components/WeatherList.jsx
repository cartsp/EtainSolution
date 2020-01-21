class WeatherList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: props.initialData,
            loading: false
        };
    }

    loadWeatherFromServer = async () => {     
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    };

    onDragStart = async () => {
        await this.setState({ loading: true });
        setTimeout(
            async function  () {
                await this.loadWeatherFromServer(); //delaying the call as the data would have updated before the loading spinner finished
                this.setState({ loading: false });
            }
            .bind(this),
            1000
        );
    }

    render() {
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];

        var weatherRows = this.state.data.map(function (weatherItem, index) {
            var roundedTemp = Math.floor(weatherItem.temperatureInC);
            var displayDate = new Date(weatherItem.date);
            var formattedDisplayDate = displayDate.getDate() + "-" + monthNames[displayDate.getMonth()] + "-" + displayDate.getFullYear();

            return (
                <WeatherItem key={index} TemperatureInC={roundedTemp} Date={formattedDisplayDate} WeatherState={weatherItem.weatherState} WeatherStateAbbreviation={weatherItem.weatherStateAbbreviation} />
            );
        });
        return (
            <div>
                <div style={this.state.loading ? {} : { display: 'none' }} className="spinner-border" role="status">
                    <span className="sr-only">Loading...</span>
                </div>
                {/* included ontouchstart to allow mobile drag to refresh */}
                <div draggable onTouchStart={(event) => this.onDragStart(event)} onDragStart={(event) => this.onDragStart(event)} className="weatherList text-center">

                    <table className="table table-bordered">
                        <thead className="thead-dark">
                            <tr>
                                <th scope="col">Date</th>
                                <th scope="col">Average Temperature</th>
                                <th scope="col">Weather Condition</th>
                                <th scope="col" />
                            </tr>
                        </thead>
                        <tbody>
                            {weatherRows}
                         </tbody>
                    </table>
                </div>
            </div>
        );
    }
}

class WeatherItem extends React.Component {
    render() {
        return (
            <tr>
                <td className="w-25">{this.props.Date}</td>
                <td className="w-25">{this.props.TemperatureInC + "\u00b0"}</td>
                <td className="w-25">{this.props.WeatherState}</td>
                <td className="w-25"><img src={"https://www.metaweather.com/static/img/weather/png/64/" + this.props.WeatherStateAbbreviation + ".png"} /></td>
            </tr>
        );
    }
}


