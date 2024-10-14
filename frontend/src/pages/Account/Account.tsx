import { Login } from "../../components/Account/login";
import { Register } from "../../components/Account/register";
import "./Account.css";

export const Account = () => {

    return (
        <div className="account-container">
            <div className="row">
                <div className="column"><Login /></div>
                <div className="column"><Register /></div>
            </div >
        </div>
    );
};