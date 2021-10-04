import React from 'react'

const Dashboard = React.lazy(() => import('./views/dashboard/Dashboard'))

const RestaurantManagement = React.lazy(() =>
  import('./views/components/management/restaurants-management/RestaurantManagement'),
)
const RestaurantsList = React.lazy(() =>
  import('./views/components/management/restaurants-list/RestaurantsList'),
)
const RestaurantCreate = React.lazy(() =>
  import('./views/components/management/restaurant-create/RestaurantCreate'),
)

const routes = [
  {path: '/', exact: true, name: 'Home'},
  {path: '/dashboard', name: 'Dashboard', component: Dashboard},
  {path: '/restaurants', name: 'Restaurants', component: RestaurantManagement},
  {path: '/restaurants/create', name: 'Create restaurant', component: RestaurantCreate}
]

export default routes
