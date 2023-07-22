interface INumberProps extends React.HTMLProps<HTMLInputElement> {

}

export default function NumberInput(props: INumberProps) {
    return (
        <input type="number" {...props} />
    )
}