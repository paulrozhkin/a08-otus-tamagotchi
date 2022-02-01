import React from 'react'

const Dashboard = React.lazy(() => import('./views/dashboard/Dashboard'))

const RestaurantManagement = React.lazy(() =>
  import('./views/components/management/restaurants-management/RestaurantManagement'),
)
const RestaurantCreate = React.lazy(() =>
  import('./views/components/management/restaurant-create/RestaurantCreate'),
)
const RestaurantUpdate = React.lazy(() =>
  import('./views/components/management/restaurant-update/RestaurantUpdate'),
)

const UsersManagement = React.lazy(() =>
  import('./views/components/management/users-management/UsersManagement'),
)
const UserCreate = React.lazy(() =>
  import('./views/components/management/user-create/UserCreate'),
)
const UserUpdate = React.lazy(() =>
  import('./views/components/management/user-update/UserUpdate'),
)

const routes = [
  {path: '/', exact: true, name: 'Home'},
  {path: '/dashboard', name: 'Dashboard', component: Dashboard},
  {path: '/restaurants', name: 'Restaurants', component: RestaurantManagement},
  {path: '/restaurants/create', name: 'Create restaurant', component: RestaurantCreate},
  {path: '/restaurants/:id/update', name: 'Update restaurant', component: RestaurantUpdate},
  {path: '/users', name: 'Users', component: UsersManagement},
  {path: '/users/create', name: 'Create user', component: UserCreate},
  {path: '/users/:id/update', name: 'Update user', component: UserUpdate}
]

export default routes
