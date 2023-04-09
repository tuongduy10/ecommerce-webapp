import { Outlet } from "react-router-dom";
import Footer from "./footer.component";
import Header from "./header.component";

const DefaultLayout = () => {
    return (
        <>
            <Header />
            <main className="main min-h-[50vh]">
                <Outlet />
            </main>
            <Footer />
        </>
    )
}

export default DefaultLayout;