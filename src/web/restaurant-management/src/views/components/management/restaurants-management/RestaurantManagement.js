import React from 'react';
import {Route, Switch} from "react-router-dom";
import RestaurantsList from "../restaurants-list/RestaurantsList";
import RestaurantCreate from "../restaurant-create/RestaurantCreate";

const RestaurantManagement = () => {
  return (
    <Switch>
      <Route exact path="/restaurants/">
        <RestaurantsList/>
      </Route>

      <Route path="/restaurants/create">
        <RestaurantCreate/>
      </Route>
    </Switch>
  );
};

export default RestaurantManagement;
