import { TextField, TextFieldProps } from "@mui/material";
import { forwardRef, useState } from "react";
import { NumericFormat, NumericFormatProps } from "react-number-format";

interface CustomProps {
    onChange: (event: any) => void;
    name: string;
}

const NumericFormatCustom = forwardRef<NumericFormatProps, CustomProps>(
    function NumericFormatCustom(props, ref) {
        const { onChange, ...other } = props;

        return (
            <NumericFormat
                {...other}
                getInputRef={ref}
                onValueChange={(values) => {
                    onChange(values.value);
                }}
                thousandSeparator
                prefix="₫"
            />
        );
    },
);

type NumberInputProps = TextFieldProps & {
    onChange?: (event: any) => void;
    label: string;
    value?: number;
    name?: string;
    size?: 'small' | 'medium'
}

export default function PriceInput(props: NumberInputProps) {
    const {
        onChange,
        label,
        value,
        name,
        size
    } = props;

    const handleOnChange = (event: { target: { value: any } }) => {
        const _value = event ? Number(event) : event;
        if (onChange !== undefined) {
            onChange(_value);
        }
    }

    return (
        <TextField
            {...props}
            label={label}
            value={value}
            size={size ?? 'small'}
            name={name}
            fullWidth
            onChange={handleOnChange}
            InputProps={{
                inputComponent: NumericFormatCustom as any,
            }}
        />
    )
}