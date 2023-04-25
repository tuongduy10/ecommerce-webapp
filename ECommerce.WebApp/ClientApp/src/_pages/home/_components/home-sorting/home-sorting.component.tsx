import { useState } from "react";
import { ICON_NAME } from "src/_shared/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shared/_components/mui-icon/mui-icon.component";


const HomeSorting = () => {
    const [isShownMenu, setIsShownMenu] = useState<boolean>(false);

    const toggleShowMenu = (toggle: boolean) => {
        setIsShownMenu(toggle);
    }

    return (
        <div className="order__item-wrapper order__button" onMouseEnter={() => toggleShowMenu(true)} onMouseOut={() => toggleShowMenu(false)}>
            <div className="order__item-dropdown">
                Tất cả <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} className={isShownMenu ? 'svg-up' : ''} />
            </div>
            <div className={`order__item-dropdown_menu ${isShownMenu ? 'open' : ''}`} onMouseEnter={() => toggleShowMenu(true)}>
                <div className="order__item-dropdown_items">
                    <a href='/' className="order__item hidden">
                        Tất cả <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                    </a>
                    <a href='/' className="order__item hidden">
                        Thời trang <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                    </a>
                    <a href='/' className="order__item hidden">
                        Công nghệ <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                    </a>
                </div>
            </div>
        </div>
    )
}

export default HomeSorting;