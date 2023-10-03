import { useState } from "react";
import { IOption, IOptionValue, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import { useProductStore } from "src/_cores/_store/root-store";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProlCategory = () => {
  const productStore = useProductStore();
  const [isExpanded, setIsExpanded] = useState(false);
  const [isFeatherIconChanged, setFeatherIconChanged] = useState(false);

  const toggleExpand = () => {
    setIsExpanded(!isExpanded);
    setFeatherIconChanged(!isFeatherIconChanged);
  };
  return (
    <div className="products__menu">
      <div className="product__filter">
        {productStore.subCategories.length > 0 && productStore.subCategories.map((sub: ISubCategory) => (
          <div key={`sub-${sub.id}`} className="filter__block">
            <h3 className="filter__title" onClick={toggleExpand}>
              {sub.name}
              <MuiIcon
                name="CHEVRON_RIGHT"
                className={`feather feather-chevron-right ${isFeatherIconChanged ? "svg-right" : ""
                  }`}
              />
            </h3>
            {sub.optionList && sub.optionList.length > 0 && (
              <ul
                className="filter__list filter--scrollbar"
                style={{
                  maxHeight: isExpanded
                    ? "calc(185px + 125 * ((100vw - 375px) / 1545))"
                    : "0px",
                }}
              >
                {sub.optionList.map((option: IOption) => (
                  <li key={`option-${option.id}`} className="filter__item">
                    <p className="mb-0">
                      {option.name}
                    </p>
                    {option.values && option.values.length > 0 && (
                      <ul className="filter__sub-list">
                        {option.values.map((_value: IOptionValue) => (
                          <li key={`option-value-${_value.id}`} className="filter__sub-item">
                            <div className="checkbox flex items-center">
                              <input
                                id="sub00"
                                className="checkboxFilter"
                                type="checkbox"
                              />
                              <label
                                htmlFor="sub00"
                                className="flex items-center mb-0"
                              >
                                <div className="sub-item-cate">{_value.name}</div>
                                <span className="sub-item-cate">({_value.totalRecord})</span>
                              </label>
                            </div>
                          </li>
                        ))}
                      </ul>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProlCategory;
