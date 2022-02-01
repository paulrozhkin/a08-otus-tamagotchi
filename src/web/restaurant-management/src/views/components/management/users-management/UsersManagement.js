import React from 'react';
import {Route, Switch} from "react-router-dom";
import UsersList from "../users-list/UsersList";
import UserCreate from "../user-create/UserCreate";
import UserUpdate from "../user-update/UserUpdate";

const UsersManagement = () => {
  return (
    <Switch>
      <Route exact path="/users/">
        <UsersList/>
      </Route>

      <Route path="/users/create">
        <UserCreate/>
      </Route>

      <Route path="/users/:id/update">
        <UserUpdate/>
      </Route>
    </Switch>
  );
};

export default UsersManagement;
