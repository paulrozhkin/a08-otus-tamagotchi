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
const RestaurantUpdate = React.lazy(() =>
  import('./views/components/management/restaurant-update/RestaurantUpdate'),
)

const routes = [
  {path: '/', exact: true, name: 'Home'},
  {path: '/dashboard', name: 'Dashboard', component: Dashboard},
  {path: '/restaurants', name: 'Restaurants', component: RestaurantManagement},
  {path: '/restaurants/create', name: 'Create restaurant', component: RestaurantCreate},
  {path: '/restaurants/:id/update', name: 'Update restaurant', component: RestaurantUpdate}
]

export default routes
