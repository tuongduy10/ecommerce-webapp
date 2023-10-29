import { Input, InputProps, TextField } from "@mui/material";
import { forwardRef, useState } from "react";

interface CustomProps {
    onChangeValue: (event: any) => void;
    name: string;
}

const InputTypeNumberCustom = forwardRef<InputProps, CustomProps>(
    function NumericFormatCustom(props, ref) {
        const { onChangeValue, ...other } = props;

        return (
            <Input
                {...other}
                onChange={(value) => {
                    onChangeValue(value);
                }}
                type="number"
            />
        );
    },
);

type NumberInputProps = {
    onChange?: (event: any) => void;
    label: string;
    value?: number;
    name?: string;
    size?: 'small' | 'medium'
}

export default function NumberInput(props: NumberInputProps) {
    const {
        onChange,
        label,
        value,
        name,
        size
    } = props;

    const handleOnChange = (event: { target: { value: any } }) => {
        const _value = event;
        if (onChange !== undefined) {
            onChange(_value);
        }
    }

    return (
        <TextField
            label={label}
            value={value}
            size={size ?? 'small'}
            name={name}
            fullWidth
            onChange={handleOnChange}
            InputProps={{
                inputComponent: InputTypeNumberCustom as any,
            }}
        />
    )
}