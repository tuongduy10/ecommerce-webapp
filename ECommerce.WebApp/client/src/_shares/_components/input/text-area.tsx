interface ITextArea extends React.HTMLProps<HTMLTextAreaElement> {

}

const TextArea = (props: ITextArea) => {
    return (
        <textarea {...props}>
            At w3schools.com you will learn how to make a website. They offer free tutorials in all web development technologies.
        </textarea>
    )
}

export default TextArea;