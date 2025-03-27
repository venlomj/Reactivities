import { useParams } from "react-router";
import { useProfile } from "../../../lib/hooks/useProfile";
import { Box, Button } from "@mui/material";
import TextInput from "../../../app/shared/components/TextInput";
import {
  editProfileSchema,
  EditProfileSchema,
} from "../../../lib/schemas/editProfileSchema";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";

type Props = {
    setEditMode: (editMode: boolean) => void;
}

export default function ProfileEditForm({setEditMode}: Props) {
  const { id } = useParams();
  const { profile, upadeProfile } = useProfile(id);

  const { control, reset, handleSubmit, formState: {isDirty, isValid} } = useForm<EditProfileSchema>({
    mode: "onTouched",
    resolver: zodResolver(editProfileSchema),
  });

  // Populate form when profile data is loaded
  useEffect(() => {
    if (profile) {
      reset({
        displayName: profile.displayName || "",
        bio: profile.bio || "",
      });
    }
  }, [profile, reset]);

  const onSubmit = (data: EditProfileSchema) => {
    upadeProfile.mutate(data, {
        onSuccess: () => setEditMode(false)
    })
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit(onSubmit)}
      display="flex"
      flexDirection="column"
      alignContent="center"
      gap={3}
      mt={3}
    >
      <TextInput label="Display name" control={control} name="displayName" />
      <TextInput
        label="Add your bio"
        control={control}
        name="bio"
        multiline
        rows={4}
      />

      <Button type="submit" variant="contained" size="large" disabled={!isValid || !isDirty || upadeProfile.isPending}>
        Update Profile
      </Button>
    </Box>
  );
}
