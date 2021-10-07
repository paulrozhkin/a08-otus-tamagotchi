import React, { Component } from 'react';
import RestaurantsMap from "./RestaurantsMap"

class Restaurants extends Component {

  constructor(props) {
    super(props);
    this.state = { restaurants: [], loading: true };
  }

  render() {
    return (
      <div>
        <h1 id="tabelLabel" >Select you restaurants</h1>

        <RestaurantsMap restaurants={this.state.restaurants} name="SomeMap" />

      </div>
    );
  }

  componentDidMount() {
    this.populateRestaurants();
  }

  async populateRestaurants() {
    // TODO - вынести базовый адрес в конфиги приложения
    const response = await fetch('https://localhost:5001/api/v1/Restaurant');
    const data = await response.json();
    this.setState({ restaurants: data.items, loading: false });
  }
}

export default Restaurants;