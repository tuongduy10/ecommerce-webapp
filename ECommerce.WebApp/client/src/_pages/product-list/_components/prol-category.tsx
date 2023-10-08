import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import { IOption, IOptionValue, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import { useProductStore } from "src/_cores/_store/root-store";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProlCategory = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const updatedSearchParams = new URLSearchParams(searchParams.toString());

  const [subCategoryId, setSubCategoryId] = useState<Number>(-1);
  const [optionValueIds, setOptionValueIds] = useState<Number[]>([]);

  const productStore = useProductStore();
  const [expandedKey, setExpandedKey] = useState('');

  const toggleExpand = (_id: number) => {
    const _key = `sub-${_id}`;
    if (_key === expandedKey) {
      updatedSearchParams.set('subCategoryId', '');
      setExpandedKey('');
    } else {
      setExpandedKey(_key);
      setOptionValueIds([]);
      updatedSearchParams.set('subCategoryId', `${_id}`);
    }
    setSearchParams(updatedSearchParams);
  };

  const generateOptionValueIds = (id: number) => {
    const currentIds = optionValueIds;
    if (!currentIds.includes(id)) {
      const ids = currentIds.concat(id);
      setOptionValueIds(ids);
    } else {
      const ids = currentIds.filter(_id => _id !== id);
      setOptionValueIds(ids);
    }
    const value = optionValueIds.join(',');
    updatedSearchParams.set('optionValueIds', value);
    setSearchParams(updatedSearchParams);
  }

  return (
    <div className="products__menu">
      <div className="product__filter">
        {productStore.subCategories.length > 0 && productStore.subCategories.map((sub: ISubCategory) => (
          <div key={`sub-${sub.id}`} className="filter__block">
            <h3 className={`filter__title ${expandedKey === `sub-${sub.id}` ? 'text-[var(--blue-dior)]' : ''}`} onClick={() => toggleExpand(sub.id)}>
              {sub.name}
              <MuiIcon
                name="CHEVRON_RIGHT"
                className={`feather feather-chevron-right ${expandedKey === `sub-${sub.id}` && (sub.optionList?.length ?? 0) > 0
                  ? "svg-right" : ""}`}
              />
            </h3>
            <ul
              className="filter__list filter--scrollbar"
              style={{
                maxHeight: expandedKey === `sub-${sub.id}`
                  ? "calc(185px + 125 * ((100vw - 375px) / 1545))"
                  : "0px",
              }}
            >
              {sub.optionList?.map((option: IOption) => (
                <li key={`option-${option.id}`} className="filter__item">
                  <p className="mb-0">
                    {option.name}
                  </p>
                  <ul className="filter__sub-list">
                    {option.values?.map((_value: IOptionValue) => (
                      <li key={`option-value-${_value.id}`} className="filter__sub-item">
                        <div className="checkbox flex items-center">
                          <input
                            checked={optionValueIds.includes(_value.id)}
                            id={`option-value-${_value.id}`}
                            className="checkboxFilter"
                            type="checkbox"
                            onChange={() => generateOptionValueIds(_value.id)}
                          />
                          <label
                            htmlFor={`option-value-${_value.id}`}
                            className="flex items-center mb-0"
                          >
                            <div className="sub-item-cate">{_value.name}</div>
                            <span className="sub-item-cate">({_value.totalRecord})</span>
                          </label>
                        </div>
                      </li>
                    ))}
                  </ul>
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProlCategory;
