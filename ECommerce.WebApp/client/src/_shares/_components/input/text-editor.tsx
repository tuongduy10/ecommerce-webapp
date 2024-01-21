import { Editor } from "@tinymce/tinymce-react";
import { MutableRefObject } from "react";
import { GlobalConfig } from "src/_configs/global.config";

interface ITextArea extends React.HTMLProps<HTMLTextAreaElement> {
    ref: MutableRefObject<any>,
}

const TextEditor = (props: ITextArea) => {
    const { ref } = props
    const init = (_event: any, editor: any) => {
        console.log(ref)
    }

    return (
        <Editor
            onInit={(evt, editor) => init(evt, editor)}
            apiKey={GlobalConfig.TINY_KEY}
            init={{
                height: 500,
                plugins: GlobalConfig.TINY_PLUGINS,
                toolbar: GlobalConfig.TINY_TOOLBAR,
            }}
        />
    )
}

export default TextEditor;