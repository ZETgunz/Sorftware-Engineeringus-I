export function hashString(input: string): string {
    const FNV_PRIME = 0x01000193;
    const FNV_OFFSET_BASIS = 0x811c9dc5;

    let hash = FNV_OFFSET_BASIS;
    for (let i = 0; i < input.length; i++) {
        hash ^= input.charCodeAt(i);
        hash *= FNV_PRIME;
    }
    return (hash >>> 0).toString(16); // Convert to unsigned 32-bit integer and then to hexadecimal string
}
