import React from 'react';
import {Route, Switch} from "react-router-dom";
import RestaurantsList from "../restaurants-list/RestaurantsList";
import RestaurantCreate from "../restaurant-create/RestaurantCreate";
import RestaurantUpdate from "../restaurant-update/RestaurantUpdate";

const RestaurantManagement = () => {
  return (
    <Switch>
      <Route exact path="/restaurants/">
        <RestaurantsList/>
      </Route>

      <Route path="/restaurants/create">
        <RestaurantCreate/>
      </Route>

      <Route path="/restaurants/:id/update">
        <RestaurantUpdate/>
      </Route>
    </Switch>
  );
};

export default RestaurantManagement;
