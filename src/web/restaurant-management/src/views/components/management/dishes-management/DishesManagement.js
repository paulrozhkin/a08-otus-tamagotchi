import React from 'react';
import {Route, Switch} from "react-router-dom";
import DishesList from "../dishes-list/DishesList";
import DishesCreate from "../dishes-create/DishesCreate";
import DishesUpdate from "../dishes-update/DishesUpdate";

const DishesManagement = () => {
  return (
    <Switch>
      <Route exact path="/dishes/">
        <DishesList/>
      </Route>

      <Route path="/dishes/create">
        <DishesCreate/>
      </Route>

      <Route path="/dishes/:id/update">
        <DishesUpdate/>
      </Route>
    </Switch>
  );
};

export default DishesManagement;
