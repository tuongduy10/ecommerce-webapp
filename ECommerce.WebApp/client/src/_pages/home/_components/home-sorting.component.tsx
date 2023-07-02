import { ClickAwayListener } from "@mui/material";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";
import { selectAlphabet, selectCategory } from "../_store/home.reducer";

const HomeSorting = (props: any) => {
    const { sortItems, sortType } = props;
    const [isToggle, setIsToggle] = useState(false);
    const [selectedName, setSelectedName] = useState('Tất cả');
    const dispatch = useDispatch();

    useEffect(() => {
        if (sortType === 'alphabet') {
            setSelectedName(sortItems[0].name);
        }
    }, [])

    const onSelect = (item: any) => {
        if (item.type === 'alphabet') {
            dispatch(selectAlphabet(item.value));
        }
        if (item.type === 'category') {
            dispatch(selectCategory(item.value));
        }
        setSelectedName(item.name);
        toggleOrderButton(false);
    }

    const toggleOrderButton = (toggle: boolean) => {
        if (toggle === isToggle) { 
            setIsToggle(toggle => !toggle);
        } else {
            setIsToggle(toggle);
        }
    }

    return (
        <ClickAwayListener onClickAway={() => setIsToggle(false)}>
            <div className="order__item-wrapper order__button">
                <div className="order__item-dropdown" onClick={() => toggleOrderButton(true)}>
                    {selectedName} <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} className={`${isToggle ? 'rotate-[-180deg]' : ''} svg`} />
                </div>
                {sortItems && sortItems.length > 0 ? (
                    <div className={`order__item-dropdown_menu w-full md:w-[124%] ${isToggle ? 'open' : ''}`}>
                        <div className="order__item-dropdown_items">
                            {sortItems.map((item: any) =>
                                <button key={item.value} className="order__item text-left hidden" onClick={() => onSelect(item)}>
                                    {item.name} <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                                </button>
                            )}
                        </div>
                    </div>
                ) : null}
            </div>
        </ClickAwayListener>
    )
}

export default HomeSorting;
