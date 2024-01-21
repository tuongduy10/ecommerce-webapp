import { useEffect } from "react"
import { useDispatch } from "react-redux";
import CommonService from "src/_cores/_services/common.service";
import { Child } from "./_components";
import { WebDirectional } from "src/_shares/_components";
import { useExampleStore } from "src/_cores/_store/root-store";
import { addData, setData } from "src/_cores/_reducers/example.reducer";

const ExamplePage = () => {
    const exampleStore = useExampleStore();
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(setData(
            [{ id: new Date().toString(), title: 'first' }]
        ));
    }, []);

    const getData = () => {
        CommonService.getData().then((res: any) => {
            dispatch(addData(res));
        }).catch((error: any) => {
            console.log(error);
        })
    }

    const set = () => {
        dispatch(addData(
            { id: new Date().toString(), title: 'new' }
        ));
    }

    return (
        <div className="custom-container">
            <WebDirectional items={[
                { path: '/example', name: 'Example' }
            ]} />
            <Child />
            {exampleStore.loading ? 'Loading...' : null}
            <button onClick={set}>Click</button>
            {exampleStore.data.length > 0 ? (
                <ul>
                    {exampleStore.data.map((item, index) => (
                        <li key={`item-${item.id}`}>{item.title}</li>
                    ))}
                </ul>
            ) : null}
        </div>
    )
}

export default ExamplePage; 