import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useSearchParams } from "react-router-dom";
import { IOption, IOptionValue, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import { setParam } from "src/_cores/_reducers/product.reducer";
import { useProductStore } from "src/_cores/_store/root-store";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProlCategory = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const updatedSearchParams = new URLSearchParams(searchParams.toString());
  
  const productStore = useProductStore();
  const dispatch = useDispatch();

  const [optionValueIds, setOptionValueIds] = useState<number[]>([]);
  const [expandedKey, setExpandedKey] = useState('');

  useEffect(() => {
    const subCategoryId = searchParams.get('subCategoryId');
    const optionValueIds = searchParams.get('optionValueIds');
    
    setExpandedKey(`sub-${subCategoryId}`);
    setOptionValueIds(optionValueIds ? optionValueIds.split(',').map(id => Number(id)) : []);
  }, []);

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
    updatedSearchParams.set('optionValueIds', '');
    const param = {
      ...productStore.param,
      subCategoryId: _id,
      optionValueIds: []
    }
    dispatch(setParam(param));
    setSearchParams(updatedSearchParams);
  };

  const generateOptionValueIds = (id?: number) => {
    const currentIds = optionValueIds;
    if (id && !currentIds.includes(id)) {
      const ids = currentIds.concat(id);
      const params = ids.join(',');
      updatedSearchParams.set('optionValueIds', params);
      setSearchParams(updatedSearchParams);
      setOptionValueIds(ids);
      const param = {
        ...productStore.param,
        optionValueIds: ids,
      }
      dispatch(setParam(param));
    } else {
      const ids = currentIds.filter(_id => _id !== id);
      const params = ids.join(',');
      updatedSearchParams.set('optionValueIds', params);
      setSearchParams(updatedSearchParams);
      setOptionValueIds(ids);
      const param = {
        ...productStore.param,
        optionValueIds: ids,
      }
      dispatch(setParam(param));
    }
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
                            checked={optionValueIds.includes(_value.id ?? -1)}
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
