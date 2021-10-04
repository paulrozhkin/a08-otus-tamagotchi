import React from 'react'
import CIcon from '@coreui/icons-react'
import {
  cilRestaurant,
  cilSpeedometer,
} from '@coreui/icons'
import { CNavGroup, CNavItem, CNavTitle } from '@coreui/react'

const _nav = [
  {
    component: CNavItem,
    name: 'Dashboard',
    to: '/dashboard',
    icon: <CIcon icon={cilSpeedometer} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Restaurants',
    to: '/restaurants',
    icon: <CIcon icon={cilRestaurant} customClassName="nav-icon" />,
  }
]

export default _nav
