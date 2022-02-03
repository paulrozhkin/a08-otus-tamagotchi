import React from 'react';
import {matchPath, Redirect, Route, Switch, useHistory, useLocation, useParams} from "react-router-dom";
import RestaurantMainInfoUpdate from "./RestaurantMainInfoUpdate";
import RestaurantTablesUpdate from "./RestaurantTablesUpdate";
import RestaurantMenuUpdate from "./ResturantMenuUpdate";
import {CDropdown, CNav, CNavItem, CNavLink, CRow} from "@coreui/react";

const RestaurantUpdate = (props) => {
  const {id} = useParams()
  const history = useHistory()
  const location = useLocation();

  const isInformationPathActive = !!matchPath(
    location.pathname,
    '/restaurants/:id/update/information'
  );

  const isTablesPathActive = !!matchPath(
    location.pathname,
    '/restaurants/:id/update/tables'
  );

  const isMenuPathActive = !!matchPath(
    location.pathname,
    '/restaurants/:id/update/menu'
  );

  function onNavigateClick(event, to) {
    event.preventDefault()
    history.push(to)
  }

  return (
    <CRow>
      <CNav variant="tabs" className="mb-3">
        <CNavItem>
          <CNavLink onClick={event => onNavigateClick(event, `/restaurants/${id}/update/information`)}
                    active={isInformationPathActive}>
            Information
          </CNavLink>
        </CNavItem>
        <CNavItem>
          <CNavLink onClick={event => onNavigateClick(event, `/restaurants/${id}/update/tables`)}
                    active={isTablesPathActive}>Tables</CNavLink>
        </CNavItem>
        <CNavItem>
          <CNavLink onClick={event => onNavigateClick(event, `/restaurants/${id}/update/menu`)}
                    active={isMenuPathActive}>Menu</CNavLink>
        </CNavItem>
      </CNav>

      <Switch>
        <Route exact path="/restaurants/:id/update">
          <Redirect to={`/restaurants/${id}/update/information`}/>
        </Route>

        <Route path="/restaurants/:id/update/information">
          <RestaurantMainInfoUpdate/>
        </Route>

        <Route path="/restaurants/:id/update/tables">
          <RestaurantTablesUpdate/>
        </Route>

        <Route path="/restaurants/:id/update/menu">
          <RestaurantMenuUpdate/>
        </Route>
      </Switch>
    </CRow>
  );
};

export default RestaurantUpdate;
