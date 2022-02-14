import {combineReducers} from "redux";
import accountReducer from "./account";
import orderReducer from "./order";

const reducers = combineReducers({
  account: accountReducer,
  order: orderReducer
});


export default reducers
