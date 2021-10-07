import React, { Component } from 'react';
import RestaurantsMap from "./RestaurantsMap"

class Restaurants extends Component {

  constructor(props) {
    super(props);
    this.state = { restaurants: [], loading: true };
  }

  render() {
    /*     let contents = this.state.loading
          ? <p><em>Loading...</em></p>
          : Restaurants.populateRestaurants(this.state.restaurants); */

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
  /*
    static populateRestaurants(restaurants) {
      return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Name</th>
              <th>Address</th>
            </tr>
          </thead>
          <tbody>
            {restaurants.map(restaurant =>
              <tr key={restaurant.id}>
                <td>{restaurant.name}</td>
                <td>{restaurant.address}</td>
              </tr>
            )}
          </tbody>
        </table>
      );
    } */

  async populateRestaurants() {
    // TODO - вынести базовый адрес в конфиги приложения
    const response = await fetch('https://localhost:5001/api/restaurants');
    const data = await response.json();
    this.setState({ restaurants: data, loading: false });
  }
}

export default Restaurants;