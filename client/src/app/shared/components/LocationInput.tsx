import {
  Box,
  debounce,
  List,
  ListItemButton,
  TextField,
  Typography,
} from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import {
  FieldValues,
  useController,
  UseControllerProps,
} from "react-hook-form";
import { LocationIQSuggestion } from "../../../lib/types";
import axios from "axios";

type Props<T extends FieldValues> = {
    label: string;
} & UseControllerProps<T>;

export default function LocationInput<T extends FieldValues>(props: Props<T>) {
  const { field, fieldState } = useController({ ...props });
  const [loading, setLoading] = useState(false);
  const [suggestions, setSuggestions] = useState<LocationIQSuggestion[]>([]);
    const [inputValue, setInputValue] = useState(field.value || '');

    useEffect(() => {
        if (field.value && typeof field.value === 'object') {
            setInputValue(field.value.venue || '')
        } else {
            setInputValue(field.value || '')
        }
    }, [field.value])


  const locationUrl="https://api.locationiq.com/v1/autocomplete?key=pk.a1891a01dc52339487292089f7158459&limit=5&dedupe=1&"

  const fetchSuggestions = useMemo(
    () => debounce(async (query: string) => {
        if (!query || query.length < 3) {
            setSuggestions([]);
            return;
        }
        setLoading(true);

        try {
            const res = await axios.get<LocationIQSuggestion[]>(`${locationUrl}q=${query}`);
            setSuggestions(res.data);
        } catch (error) {
          console.log(error)  
        } finally {
            setLoading(false)
        }
    }, 500),
     [locationUrl]);

     const handleChange = async (value: string) => {
        field.onChange(value)
        await fetchSuggestions(value)
     }

     const handleSelect = async (loaction: LocationIQSuggestion) => {
        const city = loaction.address?.city || loaction.address?.town || loaction.address?.village || loaction.display_name;
        const venue = loaction.display_name;
        const latitude = loaction.lat;
        const longitude = loaction.lon;

        setInputValue(venue);
        field.onChange({city, venue, latitude, longitude})
        setSuggestions([]);
     }

  return (
    <Box>
      <TextField
        {...props}
        value={inputValue}
        onChange={e => handleChange(e.target.value)}
        fullWidth
        variant="outlined"
        error={!!fieldState.error}
        helperText={fieldState.error?.message}
      />
      {loading && <Typography>Loading...</Typography>}
      {suggestions.length > 0 && (
        <List sx={{ border: 1 }}>
          {suggestions.map((suggestion) => (
            <ListItemButton
              divider
              key={suggestion.place_id}
              onClick={() => handleSelect(suggestion)}
            >
              {suggestion.display_name}
            </ListItemButton>
          ))}
        </List>
      )}
    </Box>
  );
}
